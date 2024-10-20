export default interface LeaderboardEntry {
    leaderBoardId: number;  // Mapa na LeaderBoardId
    userIdFk: number;       // Mapa na UserIdFk (klucz obcy do User)
    username: string;       // Nazwa użytkownika (trzeba uzyskać z obiektu `User`)
    totalPoints: number;    // Mapa na TotalPoints
}
