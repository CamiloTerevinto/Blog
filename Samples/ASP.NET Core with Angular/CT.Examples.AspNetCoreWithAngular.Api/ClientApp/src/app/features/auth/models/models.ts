export interface SignUpModel {
    email: string;
    password: string;
    repeatPassword: string;
    firstName: string;
    lastName: string;
    phoneNumber: string;
}

export interface LogInModel {
    email: string;
    password: string;
}
