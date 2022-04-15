export const ApplicationName = 'Chat App';
export const API_HOST = 'https://localhost:5111/'
export const WEB_HOST = 'http://localhost:3000/'

export const QueryParameterNames = {
  ReturnUrl: 'returnUrl',
  Provider: 'provider',
  AccessToken: 'access_token',
};

export const LoginActions = {
  LoginCallback: 'login-callback',
  ExternalLogin: 'api/v1/account/external-login'
};

export const Providers = {
  Google: 'Google',
}


export const ApplicationPaths = {
  GoogleLogin: `${API_HOST}${LoginActions.ExternalLogin}?provider=Google&returnUrl=${WEB_HOST}${LoginActions.LoginCallback}`
}