import GameResults from "../interfaces/GameResults";
import { sendGameStatistics } from "../api/gameApi";
import { useError } from "../components/ErrorContext/ErrorContext";

const useGame = () => {
    const { triggerError } = useError();
    const sendGameResults = async (gameResults: GameResults, gameType: string) => {
        try{
            const response = await sendGameStatistics(gameResults, gameType);
            return response;
        } catch(error: any){
            triggerError('Failed to send game results.');
            console.log(error);
        }
    };

    return {
        sendGameResults
    };
};

export default useGame;
