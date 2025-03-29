export interface LoginFormValues {
  email: string;
  password: string;
}

export interface RegisterFormValues extends LoginFormValues {
  username: string;
  confirmPassword: string;
}

export interface LoginResponse {
  userId: string;
  username: string;
  accessToken: string;
  refreshToken: string;
}

export interface RegisterResponse extends LoginResponse {
  email: string;
}
