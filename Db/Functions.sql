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

CREATE OR REPLACE FUNCTION check_single_achievement(
    p_user_id INT, 
    p_achievement RECORD, 
    p_achievement_criteria JSONB, 
    p_user_stat RECORD
)
RETURNS VOID AS $$
DECLARE
    meets_criteria BOOLEAN := TRUE; 
BEGIN
    IF p_achievement_criteria ? 'total_games' AND 
       (p_user_stat."total_games" < (p_achievement_criteria->>'total_games')::int) THEN
        meets_criteria := FALSE;
    END IF;

    IF p_achievement_criteria ? 'total_points' AND 
       (p_user_stat."total_points" < (p_achievement_criteria->>'total_points')::int) THEN
        meets_criteria := FALSE;
    END IF;

    IF p_achievement_criteria ? 'highest_score' AND 
       (p_user_stat."highest_score" < (p_achievement_criteria->>'highest_score')::int) THEN
        meets_criteria := FALSE;
    END IF;

    IF p_achievement_criteria ? 'lowest_time_in_seconds' AND 
       (p_user_stat."lowest_time_in_seconds" > (p_achievement_criteria->>'lowest_time_in_seconds')::int) THEN
        meets_criteria := FALSE;
    END IF;

    IF p_achievement_criteria ? 'total_traveled_distance_in_meters' AND 
       (p_user_stat."total_traveled_distance_in_meters" < (p_achievement_criteria->>'total_traveled_distance_in_meters')::int) THEN
        meets_criteria := FALSE;
    END IF;

    IF p_achievement_criteria ? 'average_score' AND 
       (p_user_stat."average_score" < (p_achievement_criteria->>'average_score')::int) THEN
        meets_criteria := FALSE;
    END IF;

    IF meets_criteria THEN
        IF NOT EXISTS (SELECT 1 FROM user_achievements WHERE "user_id" = p_user_id AND "achievement_id" = p_achievement."achievement_id") THEN
            INSERT INTO user_achievements("user_id", "achievement_id") 
            VALUES (p_user_id, p_achievement."achievement_id");
        END IF;
    END IF;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION check_achievements() 
RETURNS TRIGGER AS $$
DECLARE
    achievement RECORD;
    user_stat RECORD;
    achievement_criteria JSONB;
BEGIN
    SELECT * INTO user_stat FROM statistics WHERE "user_id" = NEW."user_id";

    FOR achievement IN SELECT * FROM achievements LOOP
        achievement_criteria := achievement."achievement_criteria"::jsonb;
        PERFORM check_single_achievement(NEW."user_id", achievement, achievement_criteria, user_stat);
    END LOOP;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;