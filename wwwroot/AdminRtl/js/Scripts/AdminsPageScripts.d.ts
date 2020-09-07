interface IAddNewAdminModel {
    name: string;
    phoneNumber: string;
    userName: string;
    password: string;
    confirmPassword: string;
    priviligs: string;
}
interface IUpdateAdminModel {
    name: string;
    priviligs: string;
}
interface IUpdateUserNameAdminModel {
    newUserName: string;
}
interface IUpdatePhoneAdminModel {
    phoneNumber: string;
}
interface IUpdatePasswordAdminModel {
    newPassword: string;
    confirmPassword: any;
}
interface IAddNewAdminErrors {
    Name: string[];
    UserName: string[];
    Password: string[];
    ConfirmPassword: string[];
    PhoneNumber: string[];
    Priviligs: string[];
    G: string;
    [key: string]: any;
}
interface IUpdateAdminErrors {
    Name: string[];
    Priviligs: string[];
    G: string;
    [key: string]: any;
}
interface IUpdateUserNameAdminErrors {
    NewUserName: string[];
    G: string;
    [key: string]: any;
}
interface IUpdatePhoneAdminErrors {
    PhoneNumber: string[];
    G: string;
    [key: string]: any;
}
interface IUpdatePasswordAdminErrors {
    NewPassword: string[];
    ConfirmPassword: string[];
    G: string;
    [key: string]: any;
}
//# sourceMappingURL=AdminsPageScripts.d.ts.map