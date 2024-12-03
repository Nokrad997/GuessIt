CREATE OR REPLACE TRIGGER update_user_statistics_trigger
    AFTER INSERT OR UPDATE ON games
    FOR EACH ROW
EXECUTE FUNCTION update_user_statistics();

CREATE OR REPLACE TRIGGER update_leaderboard_trigger
AFTER INSERT OR UPDATE ON games
FOR EACH ROW
EXECUTE FUNCTION update_leaderboard();

CREATE OR REPLACE TRIGGER after_game_insert_trigger
AFTER INSERT OR UPDATE ON statistics
FOR EACH ROW
EXECUTE FUNCTION check_achievements();