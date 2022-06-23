import React, { useEffect, useState  } from 'react';
import { API_HOST } from './auth/ApiAuthorizationConstants';
import axios from "axios";
import { ListGroup } from 'react-bootstrap';
import AuthService from '../services/auth.service';
import UserService from '../services/user.service';
import MessageItem from './MessageItem';
import { useLocation } from 'react-router-dom';

const Messages = () => {
  const [user, setUser] = useState(null);
  const [messages, setMessages] = useState([]);
  const [messageInput, setMessageInput] = useState("");

  const location = useLocation();
  const chat = location.state.chat;
  const messagesUrl = `${API_HOST}api/v1/chat/${chat.id}/message`;

  console.log("chatId:", chat.id);

  const getMessages = async () => {
    try {
      const result = await axios.get(messagesUrl, {
        headers: AuthService.authHeader()
      });
  
      console.log("getMessages: ", result);

      setMessages(result.data);
    }
    catch (err) {
      console.error(err);
    }
  }

  const sendMessage = async () => {
    try {
      const result = await axios.post(messagesUrl, {
        toId: UserService.getChatPartnerId(chat, user.id),
        text: messageInput
      }, {
        headers: AuthService.authHeader()
      });
  
      console.log("sendMessage: ", result);
    }
    catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    const signedUser = AuthService.getUser();
    if (signedUser !== null) {
      setUser(signedUser);
    }

    getMessages();
  },[]);

  const handlePostMessage = (event) => { 
    event.preventDefault();
    sendMessage();
  };

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