import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card, ListGroup, Table, Button, ButtonGroup } from 'react-bootstrap';
import LeaderboardEntry from '../../interfaces/LeaderboardEntry';
import UserStats from '../../interfaces/UserStats';
import useLeaderboard from '../../hooks/useLeaderboard';
import useUserStats from '../../hooks/useUserStats';
import useUser from '../../hooks/useUser';

const LeaderboardAndUserStats = () => {
    const [leaderboard, setLeaderboard] = useState<LeaderboardEntry[]>([]);
    const [userStats, setUserStats] = useState<UserStats | null>(null);
    const [showLeaderboard, setShowLeaderboard] = useState(true);
    const { getLeaderboard } = useLeaderboard();
    const { getUserStats } = useUserStats();
    const { fetchUsers, users } = useUser();

    useEffect(() => {
        const fetchUserData = async () => {
            await fetchUsers();
        };

        fetchUserData();
    }, []);

    useEffect(() => {
        const assignUsernamesToLeaderboard = async (leaderboard: any) => {
            return leaderboard.map((entry: { userIdFk: number; username: string; }) => {
                const matchingUser = users.find(user => user.userId === entry.userIdFk);
                entry.username = matchingUser ? matchingUser.username : "Unknown";
                return entry;
            });
        };

        const fetchLeaderboardAndStats = async () => {
            const [leaderboardData, statsData] = await Promise.all([
                getLeaderboard(),
                getUserStats(),
            ]);
            const processedLeaderboard = await assignUsernamesToLeaderboard(leaderboardData.leaderboard);
            setLeaderboard(processedLeaderboard);

            setUserStats(statsData.statistics);
        }

        fetchLeaderboardAndStats();
    }, [users]);

    return (
        <Container
            fluid
            className="d-flex flex-column justify-content-center align-items-center"
            style={{
                minHeight: '100vh',
                background: 'linear-gradient(6deg, rgba(2,0,36,1) 0%, rgba(27,61,134,1) 35%, rgba(0,212,255,1) 100%)'
            }}
        >
            <Row className="w-100 justify-content-center">
                <Col md={6} className={`mb-4 ${!showLeaderboard && 'd-none d-md-block'}`}>
                    <Card>
                        <Card.Body>
                            <h2 className="text-center mb-4">Leaderboard</h2>
                            <Table striped bordered hover>
                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>Username</th>
                                        <th>Points</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {leaderboard.length > 0 ? (
                                        leaderboard.map((entry, index) => (
                                            <tr key={entry.leaderBoardId}>
                                                <td>{index + 1}</td>
                                                <td>{entry.username}</td>
                                                <td>{entry.totalPoints}</td>
                                            </tr>
                                        ))
                                    ) : (
                                        <tr>
                                            <td colSpan={3} className="text-center">
                                                No leaderboard data available.
                                            </td>
                                        </tr>
                                    )}
                                </tbody>
                            </Table>
                        </Card.Body>
                    </Card>
                </Col>

                <Col md={6} className={`${showLeaderboard && 'd-none d-md-block'}`}>
                    <Card>
                        <Card.Body>
                            <h2 className="text-center mb-4">Your Stats</h2>
                            {userStats ? (
                                <ListGroup variant="flush">
                                    <ListGroup.Item>
                                        <h5>Total Games Played</h5>
                                        <p>{userStats.totalGames}</p>
                                    </ListGroup.Item>
                                    <ListGroup.Item>
                                        <h5>Highest Score</h5>
                                        <p>{userStats.highestScore}</p>
                                    </ListGroup.Item>
                                    <ListGroup.Item>
                                        <h5>Lowest Time (Seconds)</h5>
                                        <p>{userStats.lowestTimeInSeconds}</p>
                                    </ListGroup.Item>
                                    <ListGroup.Item>
                                        <h5>Total Traveled Distance (Meters)</h5>
                                        <p>{userStats.totalTraveledDistanceInMeters}</p>
                                    </ListGroup.Item>
                                    <ListGroup.Item>
                                        <h5>Average Score</h5>
                                        <p>{userStats.averageScore}</p>
                                    </ListGroup.Item>
                                </ListGroup>
                            ) : (
                                <p className="text-center">No stats available.</p>
                            )}
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            <ButtonGroup
                className="d-md-none mt-3 fixed-bottom pb-3"
                style={{ marginBottom: '2vh', width: '80vw', justifyContent: 'center', alignContent: 'center', left: '10vw' }}
            >
                <Button
                    variant={showLeaderboard ? "primary" : "outline-primary"}
                    onClick={() => setShowLeaderboard(true)}
                    style={{ backgroundColor: showLeaderboard ? '#1B3D86' : '#2e2e2e', color: '#fff', border: "none" }}
                >
                    Leaderboard
                </Button>
                <Button
                    variant={!showLeaderboard ? "primary" : "outline-primary"}
                    onClick={() => setShowLeaderboard(false)}
                    style={{ backgroundColor: !showLeaderboard ? '#1B3D86' : '#2e2e2e', color: '#fff', border: "none", marginLeft: '2px' }}
                >
                    Your Stats
                </Button>
            </ButtonGroup>
        </Container>
    );
};

export default LeaderboardAndUserStats;
