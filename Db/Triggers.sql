CREATE OR REPLACE TRIGGER update_user_statistics_trigger
    AFTER INSERT OR UPDATE ON games
    FOR EACH ROW
EXECUTE FUNCTION update_user_statistics();

CREATE TRIGGER update_leaderboard_trigger
AFTER INSERT OR UPDATE ON games
FOR EACH ROW
EXECUTE FUNCTION update_leaderboard();