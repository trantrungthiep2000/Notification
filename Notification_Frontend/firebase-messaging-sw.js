importScripts('https://www.gstatic.com/firebasejs/7.14.6/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/7.14.6/firebase-messaging.js');

var firebaseConfig = {
	apiKey: "AIzaSyDFw9c0ski1nklfoHi8R3qJ1-Aqg1FXZnU",
	authDomain: "testnotification-59252.firebaseapp.com",
	projectId: "testnotification-59252",
	storageBucket: "testnotification-59252.appspot.com",
	messagingSenderId: "89811850902",
	appId: "1:89811850902:web:019a1449d11d46e9c594b7",
	measurementId: "G-4JQ391T6Z9"
};

firebase.initializeApp(firebaseConfig);
const messaging=firebase.messaging();

messaging.setBackgroundMessageHandler(function (payload) {
    console.log(payload);
    const notification=JSON.parse(payload);
    const notificationOption={
        body:notification.body,
        icon:notification.icon
    };
    return self.registration.showNotification(payload.notification.title,notificationOption);
});