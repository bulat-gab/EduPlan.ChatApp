import React, { useEffect, useState  } from 'react';
import { API_HOST } from './auth/ApiAuthorizationConstants';
import axios from "axios"
import { ListGroup } from 'react-bootstrap';
import AuthService from '../services/auth.service';
import MessageItem from './MessageItem';

const chatId = "1";
const messagesUrl = `${API_HOST}api/v1/chat/${chatId}/message`;
const toId = "2";

const Messages = () => {
  const [user, setUser] = useState(null);
  const [messages, setMessages] = useState([]);
  const [messageInput, setMessageInput] = useState("");

  useEffect(() => {
    const signedUser = AuthService.getUser();
    if (signedUser !== null) {
      setUser(signedUser);
    }

    async function getMessages() {
      const result = await axios.get(messagesUrl, {
        headers: AuthService.authHeader()
      });

      setMessages(result.data);
    }
    getMessages();
  },[]);

  const handlePostMessage = (event) => { 
    event.preventDefault();
  
    // TODO: use await
    axios.post(messagesUrl, {
      toId: toId,
      text: messageInput
    }, {
      headers: AuthService.authHeader()
    })
  }

  const handleMessageInputChange = (event) => {
    event.preventDefault();
    setMessageInput(event.target.value);
  }

  if (user !== null) {
    return (
      <div>
      <h2>Hello, {user.name}</h2>
      <h3>Messages:</h3>
      <ListGroup>
        {messages ? messages.map(m => <MessageItem message={m} key={m.id} />) : 'No messages'}
      </ListGroup>
      <form onSubmit={handlePostMessage}>
        <label>
          Type your message here:
          <input type="text" name="text" onChange={handleMessageInputChange}/>
        </label>
        <input type="submit"/>
      </form>
    </div>
    )
  } else {
    return <AnonymousView />
  }
}

const AnonymousView = () => {
  return (
    <div>
      <h1>Please signin to view this page</h1>
    </div>
  )
}

export default Messages;