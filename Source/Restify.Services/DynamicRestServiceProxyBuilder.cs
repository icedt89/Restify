namespace JanHafner.Restify.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using JetBrains.Annotations;
    using Properties;
    using Request;
    using Response;

    /// <summary>
    /// Provides lite functionality for creating dynamic implementations of <see cref="IRestService"/> inheritors.
    /// </summary>
    public sealed class DynamicRestServiceProxyBuilder
    {
        [NotNull]
        private readonly Type restServiceInterfaceType;

        [NotNull]
        private readonly Type restServiceBaseType;

        [NotNull]
        private readonly TypeBuilder dynamicServiceType;

        private readonly Boolean mustImplementInterface;

        [CanBeNull]
        private Type dynamicallyGeneratedImplementation;

        [NotNull]
        private readonly AssemblyBuilder assemblyBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicRestServiceProxyBuilder"/> class.
        /// </summary>
        /// <param name="restServiceInterfaceType">The <see cref="Type"/> of the interface for which the dynamic <see cref="Type"/> is created. Mist be an inheritor of <see cref="IRestService"/>.</param>
        /// <param name="restServiceBaseType">The <see cref="Type"/> which is used as base class for the dynamically generated type. Must inherit from <see cref="RestService"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="restServiceInterfaceType"/>' cannot be null. </exception>
        private DynamicRestServiceProxyBuilder([NotNull] Type restServiceInterfaceType, [CanBeNull] Type restServiceBaseType = null)
        {
            if (restServiceInterfaceType == null)
            {
                throw new ArgumentNullException(nameof(restServiceInterfaceType));
            }

            if (!restServiceInterfaceType.IsInterface)
            {
                // MUST BE AN INTERFACE    
            }

            if (restServiceBaseType == null)
            {
                restServiceBaseType = typeof (RestService);
            }

            if (!restServiceBaseType.IsClass)
            {
                // MUST BE RestService OR INHERITOR.
            }

            // Base class already implements the interface, just create a new dynamic type with the necessary constructors.
            this.mustImplementInterface = !restServiceInterfaceType.IsAssignableFrom(restServiceBaseType);
            
            this.restServiceInterfaceType = restServiceInterfaceType;
            this.restServiceBaseType = restServiceBaseType;
            this.assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(String.Format("DynamicProxyFor.{0}", restServiceInterfaceType.FullName)), AssemblyBuilderAccess.RunAndSave); // RunAndCollect
            var dynamicModule = this.assemblyBuilder.DefineDynamicModule("Default");

            // private sealed class *Proxy : RestServiceBaseType, RestServiceInterfaceType
            this.dynamicServiceType = dynamicModule.DefineType(String.Format("{0}Proxy", restServiceInterfaceType.FullName), TypeAttributes.Sealed | TypeAttributes.NotPublic | TypeAttributes.Class, this.restServiceBaseType);
        }

        /// <summary>
        /// Factory method which creates a runtime instanciable <see cref="Type"/> which implements the supplied interface.
        /// </summary>
        /// <typeparam name="TRestServiceBase">The base type to use.</typeparam>
        /// <typeparam name="TRestServiceInterface">An interface which extends <see cref="IRestService"/>.</typeparam>
        /// <returns>The created <see cref="Type"/>.</returns>
        [NotNull]
        public static Type Proxy<TRestServiceBase, TRestServiceInterface>()
            where TRestServiceBase : RestService
        {
            var dynamicServiceProxyBuilder = new DynamicRestServiceProxyBuilder(typeof(TRestServiceInterface), typeof(TRestServiceBase));
            return dynamicServiceProxyBuilder.CreateType();
        }

        /// <summary>
        /// Creates a runtime instanciable <see cref="Type"/>.
        /// </summary>
        /// <returns>The type which implements the interface.</returns>
        [NotNull]
        public Type CreateType()
        {
            if (this.dynamicallyGeneratedImplementation != null)
            {
                return this.dynamicallyGeneratedImplementation;
            }

            // Get all visible constructors and implement each in the dynamic type. Forward each constructor to the base class constructor.
            foreach (var constructorInfo in this.restServiceBaseType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                this.DefineDynamicConstructor(constructorInfo);
            }

            if (this.mustImplementInterface)
            {
                this.dynamicServiceType.AddInterfaceImplementation(this.restServiceInterfaceType);

                // Implement each method from the interface, the schema must be:
                // 1. <TResponseBase<TRequestBase>> <Name>(<TRequestBase>)
                // 2. void <Name>(<TRequestBase>)
                foreach (var methodInfo in this.restServiceInterfaceType.GetMethods())
                {
                    this.ImplementDynamicMethod(methodInfo);
                }
            }
            
            this.dynamicallyGeneratedImplementation = this.dynamicServiceType.CreateType();
            
            this.assemblyBuilder.Save("Test.dll");
            
            return this.dynamicallyGeneratedImplementation;
        }

        /// <summary>
        /// Defines based on the a <see cref="ConstructorInfo"/> of the base type a constructor which delegates to the constructor.
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="constructorInfo"/>' cannot be null. </exception>
        private void DefineDynamicConstructor([NotNull] ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var constructorParameters = constructorInfo.GetParameters();

            var dynamicConstructor = this.dynamicServiceType.DefineConstructor(constructorInfo.Attributes, constructorInfo.CallingConvention, constructorParameters.Select(p => p.ParameterType).ToArray());
            
            // Define each constructor parameter as it is in the base class.
            for (var i = 0; i < constructorParameters.Length; i++)
            {
                var parameterInfo = constructorParameters[i];
                dynamicConstructor.DefineParameter(i, parameterInfo.Attributes, parameterInfo.Name);
            }

            // Define a public constructor which calls the constructor of the base class.
            var constructorGenerator = dynamicConstructor.GetILGenerator();

            // The first argument is not the parameter of the constructor but a reference to the current instance.
            // Because a call to the base class constructor is needed, a reference is pushed on the stack.
            constructorGenerator.Emit(OpCodes.Ldarg_0);

            // Push all other arguments of the base constructor on to the stack.
            for (var i = 1; i <= constructorParameters.Length; i++)
            {
                constructorGenerator.Emit(OpCodes.Ldarg, i);
            }

            // The second argument is the first parameter of the dynamic constructor.
            constructorGenerator.Emit(OpCodes.Call, constructorInfo);
            constructorGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Implements a single method of the interface type.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> which acts as template.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="methodInfo"/>' cannot be null. </exception>
        private void ImplementDynamicMethod([NotNull] MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodParameters = methodInfo.GetParameters().Select(p => p.ParameterType).ToList();
            this.EnsureMethodSignature(methodInfo, methodParameters);
            
            // Validation has validated that the only one parameter must inherit from RequestBase and the return value of the method must be "void" or a derivation of ResponseBase.
            var dynamicMethod = this.dynamicServiceType.DefineMethod(methodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual, methodInfo.ReturnType, methodParameters.ToArray());

            // Emit argument checks
            this.EmitArgumentValidationIfNecessary(dynamicMethod, methodInfo.GetParameters());

            if (methodInfo.ReturnType != typeof (void))
            {
                this.EmitDefaultCall(dynamicMethod, methodInfo, methodParameters);
            }
            else
            {
                this.EmitVoidCall(dynamicMethod, methodParameters);
            }

            // Because the method of the interface is implemented, there must be defined an override for the empty interface method of the dynamic type.
            // So the empty implemented interface method gets overridden by the dynamic method.
            this.dynamicServiceType.DefineMethodOverride(dynamicMethod, methodInfo);
        }

        /// <summary>
        /// Emits argument checks before calling any other code.
        /// Emits in case of String-argument <c>IsNullorEmpty</c> check.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="methodBuilder"/>' and '<paramref name="parameterInfos"/>' cannot be null. </exception>
        private void EmitArgumentValidationIfNecessary([NotNull] MethodBuilder methodBuilder, [NotNull] IEnumerable<ParameterInfo> parameterInfos)
        {
            if (methodBuilder == null)
            {
                throw new ArgumentNullException(nameof(methodBuilder));
            }

            if (parameterInfos == null)
            {
                throw new ArgumentNullException(nameof(parameterInfos));
            }

            var guardGenerator = methodBuilder.GetILGenerator();
            var local = guardGenerator.DeclareLocal(typeof(bool));

            var parameters = parameterInfos.ToArray();
            for (var i = 0; i < parameters.Length; i++)
            {
                var currentParameterInfo = parameters[i];

                // Even the attribute is declared on the interface, it was not possible for me to check for it.
                //if (currentParameterInfo.GetCustomAttribute<NotNullAttribute>() == null)
                //{
                //    continue;
                //}

                var skipThrow = guardGenerator.DefineLabel(); 

                // i = 0: "this"; we do not want "this" here.
                guardGenerator.Emit(OpCodes.Ldarg, i + 1);

                if (currentParameterInfo.ParameterType == typeof (string))
                {
                    guardGenerator.Emit(OpCodes.Call, typeof (String).GetMethod("IsNullOrEmpty", BindingFlags.Public | BindingFlags.Static, null, new[] {typeof (String)}, null));
                    guardGenerator.Emit(OpCodes.Ldc_I4_0);
                    guardGenerator.Emit(OpCodes.Ceq);
                    guardGenerator.Emit(OpCodes.Stloc, local);
                    guardGenerator.Emit(OpCodes.Ldloc, local);
                    guardGenerator.Emit(OpCodes.Brtrue_S, skipThrow);
                }
                else
                {
                    guardGenerator.Emit(OpCodes.Ldnull);
                    guardGenerator.Emit(OpCodes.Ceq);
                    guardGenerator.Emit(OpCodes.Ldc_I4_0);
                    guardGenerator.Emit(OpCodes.Ceq);
                    guardGenerator.Emit(OpCodes.Stloc, local);
                    guardGenerator.Emit(OpCodes.Ldloc, local);
                    guardGenerator.Emit(OpCodes.Brtrue_S, skipThrow);
                }

                guardGenerator.Emit(OpCodes.Ldstr, currentParameterInfo.Name);
                guardGenerator.Emit(OpCodes.Newobj, typeof(ArgumentNullException).GetConstructor(new[] { typeof(String) }));
                guardGenerator.Emit(OpCodes.Throw);
            
                guardGenerator.MarkLabel(skipThrow);
            }
        }

        /// <summary>
        /// Emits a call without a return value to the ProcessRequest generic method of the base class.
        /// </summary>
        /// <param name="methodBuilder">The <see cref="MethodBuilder"/> which gets filled with IL-instructions.</param>
        /// <param name="methodParameters">A list of <see cref="Type"/> instance which describes the parameters.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="methodBuilder"/>' and '<paramref name="methodParameters"/>' cannot be null. </exception>
        private void EmitVoidCall([NotNull] MethodBuilder methodBuilder, [NotNull] IEnumerable<Type> methodParameters)
        {
            if (methodBuilder == null)
            {
                throw new ArgumentNullException(nameof(methodBuilder));
            }
            
            if (methodParameters == null)
            {
                throw new ArgumentNullException(nameof(methodParameters));
            }

            var methodParameter = methodParameters.Single();
            var emptyResponseType = typeof(EmptyResponse<>).MakeGenericType(methodParameter);
            var call = typeof(RestService).GetMethod("ProcessRequest").MakeGenericMethod(emptyResponseType, methodParameter);

            // The same behaviour as in the constructor, first argument is a reference to the current instance.
            var dynamicMethodGenerator = methodBuilder.GetILGenerator();
            dynamicMethodGenerator.Emit(OpCodes.Ldarg_0);
            dynamicMethodGenerator.Emit(OpCodes.Ldarg_1);
            dynamicMethodGenerator.Emit(OpCodes.Call, call);

            // Remove the return value from the stack because the methods return type is void.
            dynamicMethodGenerator.Emit(OpCodes.Pop);
            dynamicMethodGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Emits a call with a return value to the ProcessRequest generic method of the base class.
        /// </summary>
        /// <param name="methodBuilder">The <see cref="MethodBuilder"/> which gets filled with IL-instructions.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> template.</param>
        /// <param name="methodParameters">A list of <see cref="Type"/> instance which describes the parameters.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="methodBuilder"/>', '<paramref name="methodInfo"/>' and '<paramref name="methodParameters"/>' cannot be null. </exception>
        private void EmitDefaultCall([NotNull] MethodBuilder methodBuilder, [NotNull] MethodInfo methodInfo, [NotNull] IEnumerable<Type> methodParameters)
        {
            if (methodBuilder == null)
            {
                throw new ArgumentNullException(nameof(methodBuilder));
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (methodParameters == null)
            {
                throw new ArgumentNullException(nameof(methodParameters));
            }

            var call = typeof(RestService).GetMethod("ProcessRequest").MakeGenericMethod(methodInfo.ReturnType, methodParameters.Single());

            // The same behaviour as in the constructor, first argument is a reference to the current instance.
            var dynamicMethodGenerator = methodBuilder.GetILGenerator();
            dynamicMethodGenerator.Emit(OpCodes.Ldarg_0);
            dynamicMethodGenerator.Emit(OpCodes.Ldarg_1);
            dynamicMethodGenerator.Emit(OpCodes.Call, call);
            dynamicMethodGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Checks the signature of the method to define.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> template.</param>
        /// <param name="methodParameters">A list of <see cref="Type"/> instance which describes the parameters.</param>
        /// <exception cref="ArgumentNullException">The value of 'methodInfo' cannot be null. </exception>
        private void EnsureMethodSignature([NotNull] MethodInfo methodInfo, [NotNull] IList<Type> methodParameters)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (methodParameters == null)
            {
                throw new ArgumentNullException(nameof(methodParameters));
            }

            if (methodParameters.Count == 1 && !typeof(RequestBase).IsAssignableFrom(methodParameters.Single()))
            {
                throw new ArgumentException(Exceptions.TheOnlyOneParameterMustInheritFromRequestBaseMessage);
            }

            if (!typeof(ResponseBase).IsAssignableFrom(methodInfo.ReturnType) && methodInfo.ReturnType != typeof(void))
            {
                throw new ArgumentException(Exceptions.ReturnValueMustBeOfTypeVoidOrInheritFromResponseBaseMessage);
            }
        }
    }
}