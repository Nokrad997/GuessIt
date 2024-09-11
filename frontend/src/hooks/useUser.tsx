import { useState } from 'react';
import { getUserData, updateUserData, deleteUserAccount } from '../api/userApi';
import User from '../interfaces/UserData';


const useUser = () => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchUserData = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getUserData();
      setUser(data);
    } catch (err) {
      setError('Failed to fetch user data');
      throw new Error("");
    } finally {
      setLoading(false);
    }
  };

  const editUserData = async (updatedData: Partial<User>) => {
    setLoading(true);
    setError(null);
    try {
      const updatedUser = await updateUserData(updatedData); 
      setUser(updatedUser);
    } catch (err) {
      setError('Failed to update user data');
      throw new Error("");
    } finally {
      setLoading(false);
    }
  };

  const removeUserAccount = async () => {
    setLoading(true);
    setError(null);
    try {
      await deleteUserAccount(user!); 
      setUser(null);
    } catch (err) {
      setError('Failed to delete user account');
      throw new Error("");
    } finally {
      setLoading(false);
    }
  };

  return {
    user,
    loading,
    error,
    fetchUserData,
    editUserData,
    removeUserAccount,
  };
};

export default useUser;
