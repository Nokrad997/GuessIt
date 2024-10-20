export default interface UserStats {
    statisticId: number;                    // Mapa na StatisticId
    userIdFk: number;                       // Mapa na UserIdFk (klucz obcy do User)
    totalGames: number;                     // Mapa na TotalGames
    totalPoints: number;                    // Mapa na TotalPoints
    highestScore: number;                   // Mapa na HighestScore
    lowestTimeInSeconds: number;            // Mapa na LowestTimeInSeconds
    totalTraveledDistanceInMeters: number;  // Mapa na TotalTraveledDistanceInMeters
    averageScore: number;                   // Mapa na AverageScore
}
