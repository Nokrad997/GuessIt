import User from '../interfaces/UserData';
import { api } from './axios';

export const getUserData = async () => {
  try {
    const response = await api.get('User/user-data-based-on-token');
    console.log(response.data)
    return response.data.user;
  } catch (error: any) {
    if (error.response) {
      console.error("Failed to fetch user data:", error.response.status, error.response.data);
      throw new Error(error.response.data.error || "Failed to fetch user data");
    } else {
      console.error("Error during request:", error.message);
      throw new Error("Failed to fetch user data");
    }
  }
};

export const updateUserData = async (updatedData: Partial<User>) => {
  try {
    const response = await api.put(`User/${updatedData.userId}`, updatedData);
    return response.data;
  } catch (error: any) {
    if (error.response) {
      console.error("Failed to update user data:", error.response.status, error.response.data);
      throw new Error(error.response.data.error || "Failed to update user data");
    } else {
      console.error("Error during request:", error.message);
      throw new Error("Failed to update user data");
    }
  }
};

export const deleteUserAccount = async (userData: User) => {
  try {
    await api.delete(`User/${userData.userId}`);
  } catch (error: any) {
    if (error.response) {
      console.error("Failed to delete user account:", error.response.status, error.response.data);
      throw new Error(error.response.data.error || "Failed to delete user account");
    } else {
      console.error("Error during request:", error.message);
      throw new Error("Failed to delete user account");
    }
  }
};
