INSERT INTO achievements (achievement_id, achievement_name, achievement_description, achievement_criteria, created_at, updated_at)
VALUES
    (1, 'Beginner''s Luck', 'Play your first game.', '{"TotalGames": 1}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (2, 'Globetrotter', 'Play 100 games.', '{"TotalGames": 100}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (3, 'Seasoned Explorer', 'Play 500 games.', '{"TotalGames": 500}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (4, 'Points Collector', 'Accumulate a total of 1,000 points.', '{"TotalPoints": 1000}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (5, 'Master Scorer', 'Score a total of 10,000 points.', '{"TotalPoints": 10000}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (6, 'Perfectionist', 'Achieve a highest score of 5,000 points in a single game.', '{"HighestScore": 5000}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (7, 'Speed Demon', 'Guess a location in under 10 seconds.', '{"LowestTimeInSeconds": 10}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (8, 'Fast and Accurate', 'Guess location in under 30 seconds with a score of over 4,000.', '{"LowestTimeInSeconds": 30, "AverageScore": 4000}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (9, 'World Traveler', 'Travel a total distance of 10,000 km in all games.', '{"TotalTraveledDistanceInMeters": 10000000}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (10, 'Explorer Extraordinaire', 'Travel a total distance of 100,000 km in all games.', '{"TotalTraveledDistanceInMeters": 100000000}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (11, 'Steady Hand', 'Maintain an average score of over 3,000 across 10 games.', '{"AverageScore": 3000, "TotalGames": 10}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
    (12, 'Master of Geography', 'Maintain an average score of over 4,500 across 100 games.', '{"AverageScore": 4500, "TotalGames": 100}', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
