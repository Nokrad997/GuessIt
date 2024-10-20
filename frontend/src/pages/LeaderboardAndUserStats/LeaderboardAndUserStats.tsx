import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card, ListGroup, Table } from 'react-bootstrap';
import LeaderboardEntry from '../../interfaces/LeaderboardEntry';
import UserStats from '../../interfaces/UserStats';
import useUserStats from '../../hooks/useUserStats';
import useLeaderboard from '../../hooks/useLeaderboard';

const LeaderboardAndUserStats = () => {
    const [leaderboard, setLeaderboard] = useState<LeaderboardEntry[]>([]);
    const [userStats, setUserStats] = useState<UserStats | null>(null);

    const { getLeaderboard } = useLeaderboard();
    const { getUserStats } = useUserStats();

    // Fetch leaderboard data
    useEffect(() => {
        const fetchLeaderboard = async () => {
            const leaderboardData = await getLeaderboard();
            setLeaderboard(leaderboardData);
        };

        const fetchUserStats = async () => {
            const statsData = await getUserStats();
            setUserStats(statsData);
        };

        fetchLeaderboard();
        fetchUserStats();
    }, []);

    return (
        <Container
            fluid
            className="d-flex justify-content-center align-items-center"
            style={{
                minHeight: '100vh',
                background: 'linear-gradient(6deg, rgba(2,0,36,1) 0%, rgba(27,61,134,1) 35%, rgba(0,212,255,1) 100%)'
            }}
        >
            <Row className="w-100 justify-content-center">
                {/* Leaderboard Section */}
                <Col md={6} className="mb-4">
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

                {/* User Stats Section */}
                <Col md={6}>
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
        </Container>
    );
};

export default LeaderboardAndUserStats;
