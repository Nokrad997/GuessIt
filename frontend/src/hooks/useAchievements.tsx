import { GetAchievements } from "../api/achievementsApi";
import { useError } from "../components/ErrorContext/ErrorContext";
const useAchievements = () => {
    const { triggerError } = useError();
    const getUserAchievements = async () => {
        try{
            const response = await GetAchievements();
            return response;
        } catch(error: any){
            triggerError('Failed to send game results.');
            console.log(error);
        }
    };
    return {
        getUserAchievements
    };
};
export default useAchievements;