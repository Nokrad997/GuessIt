
import { GetStatistics } from '../api/userStatsApi';
import { useError } from '../components/ErrorContext/ErrorContext';
const useUserStats = () => {
    const { triggerError } = useError();
    const getUserStats = async () => {
        try {
            const response = GetStatistics()
            return response;
        } catch (error) {
            console.error('Failed to fetch user stats:', error);
            triggerError("Failed to fetch user stats");
            return null;
        }
    };
    return { getUserStats };
};
export default useUserStats;