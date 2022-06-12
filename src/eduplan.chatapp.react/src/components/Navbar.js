import NavbarPrivate from './NavbarPrivate';
import NavbarPublic from './NavbarPublic';
import React from 'react'
import AuthService from '../services/auth.service';

export default function Navbar() {
  const isAuthenticated = AuthService.isAuthenticated();  

  return (
    isAuthenticated ? <NavbarPrivate /> : <NavbarPublic />
  )
}
