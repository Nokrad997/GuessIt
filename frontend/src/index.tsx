import React from 'react';
import ReactDOM from 'react-dom/client'; // Use ReactDOM from 'react-dom/client' for React 18+
import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';
import App from './App'; // Make sure this references your App.tsx file
import { BrowserRouter } from 'react-router-dom';
import { ErrorProvider } from './components/ErrorContext/ErrorContext';

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
root.render(
  <React.StrictMode>
    <ErrorProvider>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </ErrorProvider>
  </React.StrictMode>
);