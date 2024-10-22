import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card, ListGroup } from 'react-bootstrap';
import UserAchievement from '../../interfaces/UserAchievement';
import useAchievements from '../../hooks/useAchievements';

const UserAchievements = () => {
    const [achievements, setAchievements] = useState<UserAchievement[]>([]);
    const { getUserAchievements } = useAchievements();
    useEffect(() => {
        const getAchievements = async () => {
            const result = await getUserAchievements();
            setAchievements(result.userAchievements); 
        };
        getAchievements();
    }, [getUserAchievements]);
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
                <Col md={6}>
                    <Card>
                        <Card.Body>
                            <h2 className="text-center mb-4">Your Achievements</h2>
                            <ListGroup variant="flush">
                                {achievements.length > 0 ? (
                                    achievements.map((achievement) => (
                                        <ListGroup.Item key={achievement.achievementId}>
                                            <h5>{achievement.achievementName}</h5>
                                            <p>{achievement.achievementDescription}</p>
                                        </ListGroup.Item>
                                    ))
                                ) : (
                                    <p className="text-center">No achievements to display.</p>
                                )}
                            </ListGroup>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
};
export default UserAchievements;