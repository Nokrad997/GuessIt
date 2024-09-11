import React, { createContext, useState, useContext, ReactNode } from 'react';

interface ErrorContextProps {
  triggerError: (message: string) => void;
}

interface ErrorProviderProps {
  children: ReactNode;  // Define the children prop
}

const ErrorContext = createContext<ErrorContextProps | undefined>(undefined);

export const useError = () => {
  const context = useContext(ErrorContext);
  if (!context) {
    throw new Error('useError must be used within an ErrorProvider');
  }
  return context;
};

export const ErrorProvider: React.FC<ErrorProviderProps> = ({ children }) => {
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const triggerError = (message: string) => {
    setErrorMessage(message);

    // Hide the error after 5 seconds
    setTimeout(() => {
      setErrorMessage(null);
    }, 10000);
  };

  return (
    <ErrorContext.Provider value={{ triggerError }}>
      {children}

      {/* Popup Component for displaying errors */}
      {errorMessage && (
        <div style={popupStyle}>
          {errorMessage}
        </div>
      )}
    </ErrorContext.Provider>
  );
};

const popupStyle = {
  position: 'fixed' as 'fixed',
  top: '20px',
  left: '50%',
  transform: 'translateX(-50%)',
  backgroundColor: '#f44336',
  color: 'white',
  padding: '15px',
  borderRadius: '5px',
  zIndex: 1000,
  boxShadow: '0px 4px 6px rgba(0, 0, 0, 0.1)',
};
