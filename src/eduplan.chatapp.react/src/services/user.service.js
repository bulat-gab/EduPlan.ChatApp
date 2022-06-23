const getChatPartnerId = (chat, myId) => {
  const id1 = chat.user1.id;
  const id2 = chat.user2.id;

  
  
  console.log("myId: ", myId);

  if (myId == id1) {
    console.log("id2: ", id2);
    return id2;
  }
  else if (myId == id2) {
    console.log("id1: ", id1);
    return id1;
  }

};

const UserService = {
  getChatPartnerId
};

export default UserService;