import React, { useEffect, useState  } from 'react';
import { BackendApiAddress } from './auth/ApiAuthorizationConstants';
import axios from "axios"
import { ListGroup } from 'react-bootstrap';

const chatId = "1";
const messagesUrl = `${BackendApiAddress}api/v1/chat/${chatId}/message`;
const toId = "2";

function Messages(props) {
  const token = props.token;
  const headers = {
    "Authorization": `bearer ${token}`,
    "Access-Control-Allow-Origin": "*",
  };

  const [messages, setMessages] = useState();
  
  useEffect(() => {
    async function getMessages() {
      const result = await axios.get(messagesUrl, {
        headers: headers
      });

      setMessages(result.data);
    }
    getMessages();
  },[]);

  console.log(messages);

  const listMessages = messages != null ? messages.map((message) => {
    let formattedDate = new Date(message.createdAt + "Z");
    formattedDate = formattedDate.toLocaleString();
    console.log(formattedDate);

    return (
      <ListGroup.Item
      key={message.id}
      variant={message.fromId == "1" ? "primary" : "success"}
    >
      Date: {formattedDate} <br/>
      FromUserId: {message.fromId} <br/>
      Text: {message.text}
    </ListGroup.Item>
    )
  }) : "No messages";

  const handlePostMessage = (event) => {
    event.preventDefault();
  
    console.log(event.target.value);
    axios.post(messagesUrl, {
      toId: toId,
      text: "How are you?"
    },
    {
      headers: headers
    })
  }

  return (
    <div>
      <h2>Messages:</h2>
      <ListGroup>
        {listMessages}
      </ListGroup>
      <form onSubmit={handlePostMessage}>
        <label>
          Type your message here:
          <input type="text"/>
        </label>
        <input type="submit"/>
      </form>
    </div>
  )
}

export default Messages;