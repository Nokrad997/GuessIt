import React, { useEffect, useState } from 'react';
import { Form, Button, Container, Row, Col, Card, Alert, Spinner, Modal } from 'react-bootstrap';
import useUser from '../../hooks/useUser';
import useAuth from '../../hooks/useAuth';
import { useNavigate } from 'react-router-dom';

const ProfilePage = () => {
    const { user, loading, error, fetchUserData, editUserData, removeUserAccount } = useUser();
    const { logoutCustomer } = useAuth();
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [passwordError, setPasswordError] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const [showDeleteModal, setShowDeleteModal] = useState(false);

    useEffect(() => {
        fetchUserData();
    }, []);

    useEffect(() => {
        if (user) {
            setEmail(user.email);
            setUsername(user.username);
        }
    }, [user]);

    const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value);
    };

    const handleUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setUsername(e.target.value);
    };

    const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
        if (e.target.value === '') {
            setConfirmPassword('');
        }
    };

    const handleConfirmPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setConfirmPassword(e.target.value);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setPasswordError(null);
        setSuccessMessage(null);

        if (password && password !== confirmPassword) {
            setPasswordError('Passwords do not match.');
            return;
        }

        const updatedData: Partial<typeof user> = { email, username };
        if (password) {
            (updatedData as any).password = password;
        }

        try {
            await editUserData(updatedData);
            setSuccessMessage('Profile updated successfully!');
        } catch (error) {
            setPasswordError('Failed to update profile');
        }
    };

    const handleDeleteAccount = async () => {
        try {
            await removeUserAccount();
            logoutCustomer();
            navigate('/');
        } catch (error) {
            setPasswordError('Failed to delete account.');
        }
    };

    return (
        <Container
            fluid
            className="d-flex justify-content-center align-items-center"
            style={{
                minHeight: '100vh',
                background: 'linear-gradient(6deg, rgba(2,0,36,1) 0%, rgba(27,61,134,1) 35%, rgba(0,212,255,1) 100%)',
            }}
        >
            <Row className="w-100 justify-content-center">
                <Col md={6}>
                    <Card>
                        <Card.Body>
                            <h2 className="text-center mb-4">Manage Profile</h2>

                            {loading && (
                                <div className="text-center mb-3">
                                    <Spinner animation="border" />
                                </div>
                            )}

                            {error && <Alert variant="danger">{error}</Alert>}

                            {successMessage && <Alert variant="success">{successMessage}</Alert>}

                            <Form onSubmit={handleSubmit}>
                                <Form.Group controlId="formEmail" className="mb-3">
                                    <Form.Label>Email</Form.Label>
                                    <Form.Control
                                        type="email"
                                        value={email || ''}
                                        onChange={handleEmailChange}
                                        placeholder="Enter new email"
                                        required
                                    />
                                </Form.Group>

                                <Form.Group controlId="formUsername" className="mb-3">
                                    <Form.Label>Username</Form.Label>
                                    <Form.Control
                                        type="text"
                                        value={username || ''}
                                        onChange={handleUsernameChange}
                                        placeholder="Enter new username"
                                        required
                                    />
                                </Form.Group>

                                <Form.Group controlId="formPassword" className="mb-3">
                                    <Form.Label>New Password</Form.Label>
                                    <Form.Control
                                        type="password"
                                        value={password || ''}
                                        onChange={handlePasswordChange}
                                        placeholder="Enter new password"
                                    />
                                </Form.Group>

                                {password && (
                                    <Form.Group controlId="formConfirmPassword" className="mb-3">
                                        <Form.Label>Confirm Password</Form.Label>
                                        <Form.Control
                                            type="password"
                                            value={confirmPassword || ''}
                                            onChange={handleConfirmPasswordChange}
                                            placeholder="Confirm new password"
                                        />
                                    </Form.Group>
                                )}

                                {passwordError && <Alert variant="danger">{passwordError}</Alert>}

                                <Button variant="primary" type="submit" className="w-100" disabled={loading}>
                                    Save Changes
                                </Button>

                                <Button
                                    variant="danger"
                                    className="w-100 mt-3"
                                    onClick={() => setShowDeleteModal(true)}
                                    disabled={loading}
                                >
                                    Delete Account
                                </Button>
                            </Form>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirm Account Deletion</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    Are you sure you want to delete your account? This action is irreversible.
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
                        Cancel
                    </Button>
                    <Button variant="danger" onClick={handleDeleteAccount}>
                        Delete Account
                    </Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default ProfilePage;
