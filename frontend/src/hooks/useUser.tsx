import { useState } from 'react';
import { getUserData, updateUserData, deleteUserAccount, getUsers } from '../api/userApi';
import User from '../interfaces/UserData';


const useUser = () => {
  const [user, setUser] = useState<User | null>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchUsers = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getUsers();
      console.log(data)
      setUsers(data);
    } catch (err) {
      setError('Failed to fetch users');
      throw new Error("");
    } finally {
      setLoading(false);
    }
  };

  const fetchUserData = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getUserData();
      console.log(data)
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
    users,
    loading,
    error,
    fetchUsers,
    fetchUserData,
    editUserData,
    removeUserAccount,
  };
};

export default useUser;
