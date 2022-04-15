import React, { useEffect } from 'react';
import { Button } from 'react-bootstrap';
import { ApplicationPaths, QueryParameterNames, WEB_HOST } from './ApiAuthorizationConstants';

const getQueryVariable = (variable) => {
  var query = window.location.search.substring(1);
  var vars = query.split('&');
  for (var i = 0; i < vars.length; i++) {
      var pair = vars[i].split('=');
      if (decodeURIComponent(pair[0]) == variable) {
          return decodeURIComponent(pair[1]);
      }
  }
  console.log('Query variable %s not found', variable);
}

const LoginMenu = (props) => {

  useEffect(() => {
    const url = window.location.href;
    console.log('Url: ' + url);
    const accessToken = getQueryVariable(QueryParameterNames.AccessToken);
    if (accessToken != null){
      props.onUserLogin(accessToken);
      
      const redirectUrl = WEB_HOST;
      window.location.replace(redirectUrl);
    }
    else{
      console.log('token is empty.');
    }
    
  },[])
  
  return (
    <a href={ApplicationPaths.GoogleLogin}>
      <Button>
        Login with Google
      </Button>
    </a>
    )
}

export default LoginMenu;