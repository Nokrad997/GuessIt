import React from 'react';
import './App.css';
import { Route, Routes } from 'react-router-dom';
import { LoadScriptNext } from '@react-google-maps/api'; // Import LoadScriptNext
import LoginPage from './pages/Login/Login';
import RegisterPage from './pages/Register/Register';
import HomePage from './pages/Home/Home';
import CustomNavbar from './components/Navbar/Navbar';
import Profile from './pages/Profile/Profile';
import Location from './pages/Location/Location';
import Game from './pages/Game/Game';

function App() {
  return (
    <LoadScriptNext
      googleMapsApiKey={process.env.REACT_APP_GOOGLE_MAPS_API_KEY || ''} // Replace with your API key
      libraries={['places', 'geometry']} // Include any additional libraries if needed
      loadingElement={<div>Loading Google Maps...</div>} // Loading placeholder
    >
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
            <Route path="/game-dashboard" element={<Location />} />
            <Route path="game-dashboard/game" element={<Game />} />
          </Routes>
        </div>
      </div>
    </LoadScriptNext>
  );
}

export default App;
