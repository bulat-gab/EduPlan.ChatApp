import React from 'react';
import { Button } from 'react-bootstrap';
import { ApplicationPaths } from './ApiAuthorizationConstants';

export function LoginMenu() {
  return (
    <a href={ApplicationPaths.GoogleLogin}>
      <Button>
        Login with Google
      </Button>
    </a>
    )
}

export default LoginMenu;