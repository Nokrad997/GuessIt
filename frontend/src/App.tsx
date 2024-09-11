import React from 'react';
import './App.css';
import { Route, Routes } from 'react-router-dom';
import LoginPage from './pages/Login/Login';
import RegisterPage from './pages/Register/Register';
import HomePage from './pages/Home/Home';
import CustomNavbar from './components/Navbar/Navbar';
import Profile from './pages/Profile/Profile';

function App() {

  return (
    <div className="App">
      <div className="Navbar">
        <CustomNavbar />
      </div>
      <div className="Content">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/profile-management" element={<Profile />} />
          <Route path="/game-dashboard" element={<RegisterPage />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
