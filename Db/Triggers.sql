-- func for calculating time difference in seconds
CREATE OR REPLACE FUNCTION calculate_time_diff_in_seconds(
    start_time TIMESTAMPTZ,
    end_time TIMESTAMPTZ
) RETURNS NUMERIC AS $$
DECLARE
    time_diff_in_seconds NUMERIC;
BEGIN
    time_diff_in_seconds := EXTRACT(EPOCH FROM (end_time - start_time));
    RETURN time_diff_in_seconds;
END;
$$ LANGUAGE plpgsql;


-- func and trigger for statistics
CREATE OR REPLACE FUNCTION update_user_statistics() RETURNS TRIGGER AS $$
DECLARE
    time_diff_in_seconds NUMERIC;
    statistics_exist BOOLEAN;
BEGIN
    SELECT EXISTS (
        SELECT 1 FROM statistics WHERE user_id = NEW.user_id
    ) INTO statistics_exist;

    IF NOT statistics_exist THEN
        INSERT INTO statistics (user_id, total_games, total_points, highest_score, lowest_time_in_seconds, total_traveled_distance_in_meters, average_score)
        VALUES (NEW.user_id, 0, 0, 0, 0, 0, 0);
    END IF;

    UPDATE statistics
    SET total_games = total_games + 1,
        total_points = total_points + NEW.score
    WHERE user_id = NEW.user_id;

    UPDATE statistics
    SET highest_score = NEW.score
    WHERE user_id = NEW.user_id
      AND NEW.score > (SELECT highest_score FROM statistics WHERE user_id = NEW.user_id);

    time_diff_in_seconds := calculate_time_diff_in_seconds(NEW.start_time, NEW.end_time);

    UPDATE statistics
    SET lowest_time_in_seconds = time_diff_in_seconds
    WHERE user_id = NEW.user_id
      AND time_diff_in_seconds < lowest_time_in_seconds;

    UPDATE statistics
    SET total_traveled_distance_in_meters = total_traveled_distance_in_meters + NEW.distance_to_starting_location * 1000
    WHERE user_id = NEW.user_id;

    UPDATE statistics
    SET average_score = total_points / total_games
    WHERE user_id = NEW.user_id;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER update_user_statistics_trigger
    AFTER INSERT OR UPDATE ON games
    FOR EACH ROW
EXECUTE FUNCTION update_user_statistics();


-- func and trigger for leaderboard
CREATE OR REPLACE FUNCTION update_leaderboard()
RETURNS TRIGGER AS $$
DECLARE
    current_total_points INTEGER;
BEGIN
    SELECT total_points INTO current_total_points
    FROM leaderboard
    WHERE user_id = NEW.user_id;

    IF current_total_points IS NOT NULL THEN
        UPDATE leaderboard
        SET total_points = current_total_points + NEW.score
        WHERE user_id = NEW.user_id;
    ELSE
        INSERT INTO leaderboard (user_id, total_points)
        VALUES (NEW.user_id, NEW.score);
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_leaderboard_trigger
AFTER INSERT OR UPDATE ON games
FOR EACH ROW
EXECUTE FUNCTION update_leaderboard();
