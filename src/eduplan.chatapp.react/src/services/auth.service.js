import jwt_decode from "jwt-decode";

const UserKey = 'user';

export const AuthenticationResultStatus = {
  Redirect: 'redirect',
  Success: 'success',
  Fail: 'fail'
};

const signin = (accessToken) => {
  if (accessToken == null) {
    return {
      status: AuthenticationResultStatus.Fail,
      message: 'Access Token is null'
    };
  }

  const decoded = jwt_decode(accessToken);
  const user = {
    id: decoded.nameid,
    name: decoded.name,
    email: decoded.email,
    accessToken: accessToken,
  };
  localStorage.setItem(UserKey, JSON.stringify(user));

  return { status: AuthenticationResultStatus.Success };
}

const logout = () => {
  localStorage.removeItem(UserKey);
}

const getUser = () => {
  return JSON.parse(localStorage.getItem(UserKey)); 
}

const authHeader = () => {
  const user = JSON.parse(localStorage.getItem(UserKey));
  if (user && user.accessToken) {
    return {
      Authorization: 'Bearer ' + user.accessToken,
      "Access-Control-Allow-Origin": "*",
    };
  } else {
    return {};
  }
}

const isAuthenticated = () => {
  const user = JSON.parse(localStorage.getItem(UserKey));
  return user !== null;
}
 
const AuthService = {
  signin,
  logout,
  getUser,
  authHeader,
  isAuthenticated,
}

export default AuthService;



