declare let operations: {
    _loginUrl: string;
    loginForm: JQuery<HTMLElement>;
    generalErrorEL: JQuery<HTMLElement>;
    loginBtnSubmit: JQuery<HTMLElement>;
    userNameInpt: JQuery<HTMLElement>;
    passwordInpt: JQuery<HTMLElement>;
    PostedFormOfSignIn: JQuery<HTMLElement>;
    setCustomHtml5InputsRequiredTitle(): void;
    initWork(): void;
    getErrorsContainer(errors: string[]): JQuery<HTMLSpanElement> | JQuery<DocumentFragment>;
    SignInPost(model: any, rememberMe: string): void;
    handleLoginForm(): void;
    storeUserToken(token: string): void;
    start(): void;
};
//# sourceMappingURL=loginPageScript.d.ts.map