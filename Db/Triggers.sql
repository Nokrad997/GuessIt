CREATE OR REPLACE TRIGGER update_user_statistics_trigger
    AFTER INSERT OR UPDATE ON games
    FOR EACH ROW
EXECUTE FUNCTION update_user_statistics();

CREATE TRIGGER update_leaderboard_trigger
AFTER INSERT OR UPDATE ON games
FOR EACH ROW
EXECUTE FUNCTION update_leaderboard();

CREATE TRIGGER after_game_insert
AFTER INSERT OR UPDATE ON statistics
FOR EACH ROW
EXECUTE FUNCTION check_single_achievement();