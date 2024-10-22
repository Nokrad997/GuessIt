export default interface UserStats {
    statisticId: number;                   
    userIdFk: number;                      
    totalGames: number;                    
    totalPoints: number;                   
    highestScore: number;                  
    lowestTimeInSeconds: number;           
    totalTraveledDistanceInMeters: number; 
    averageScore: number;                  
}