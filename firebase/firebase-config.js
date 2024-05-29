import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";

const firebaseConfig = {
    apiKey: "AIzaSyCVD9tYEmwjf7tZyFYKcJ0b9G6ToT46ing",
    authDomain: "teste-iatec.firebaseapp.com",
    projectId: "teste-iatec",
    storageBucket: "teste-iatec.appspot.com",
    messagingSenderId: "1025647526776",
    appId: "1:1025647526776:web:22bc33d2edf147f266be8d",
    measurementId: "G-NM6YZZWZS4",
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

export { auth };
