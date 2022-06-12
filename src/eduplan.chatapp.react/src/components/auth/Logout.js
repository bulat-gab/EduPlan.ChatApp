import React from 'react';
import AuthService from '../../services/auth.service';

export default function Logout() {
  AuthService.logout();
  console.log("User has logged out");

  return null;
}
