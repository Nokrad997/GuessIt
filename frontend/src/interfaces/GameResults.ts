export default interface GameResults {
    StartLocation: {
        type: string;
        coordinates: number[];
    }
    GuessedLocation: {
        type: string;
        coordinates: number[];
    }
    DistanceToStartingLocation: number;
    TraveledDistance: number;
    StartTime: Date;
    EndTime: Date;
}