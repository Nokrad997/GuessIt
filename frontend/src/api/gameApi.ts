import GameResults from "../interfaces/GameResults";
import { api } from "./axios";


export const sendGameStatistics = async (gameResults: GameResults, gameType: string) => {
    try {
        const response = await api.post(`Game/after-game-statistics/${gameType}`, gameResults);
        console.log(response.data);
    } catch (error: any) {
        console.error("Failed to send game results:", error.response.status, error.response.data);
        throw new Error(error.response.data.error || "Failed to send game results");
    }
};