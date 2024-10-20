import { retrieveLeaderboard } from '../api/leaderboardApi';
import { GetStatistics } from '../api/userStatsApi';
import { useError } from '../components/ErrorContext/ErrorContext';

const useLeaderboard = () => {
    const { triggerError } = useError();
    const getLeaderboard = async () => {
        try {
            const response = retrieveLeaderboard()
            return response;
        } catch (error) {
            console.error('Failed to fetch leaderboard:', error);
            triggerError("Failed to fetch leaderboard");
            return null;
        }
    };

    return { getLeaderboard };
};

export default useLeaderboard;
