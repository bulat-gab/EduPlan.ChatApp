export const ApplicationName = 'Chat App';
export const BackendApiAddress = 'https://localhost:5111/'
export const FrontendApiAddress = 'http://localhost:3000/'

export const QueryParameterNames = {
  ReturnUrl: 'returnUrl',
  Provider: 'provider'
};

export const LoginActions = {
  LoginCallback: 'login-callback',
  ExternalLogin: 'api/v1/account/external-login'
};

export const Providers = {
  Google: 'Google',
}

export const ApplicationPaths = {
  GoogleLogin: `${BackendApiAddress}${LoginActions.ExternalLogin}?provider=Google&returnUrl=${FrontendApiAddress}`
}