import AuthService from '../../services/auth.service';

export default function Logout() {
  AuthService.logout();
  console.log("User has logged out");
  location.reload(true);

  return null;
}
