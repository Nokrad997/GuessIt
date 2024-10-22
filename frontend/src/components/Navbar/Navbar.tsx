import React, { useState, useEffect } from 'react';
import { Navbar, Nav, Container, Row, Col, Button, DropdownButton, Dropdown } from 'react-bootstrap';
import logo from '../../assets/logo_NoBackground.png';
import { useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';

const CustomNavbar = () => {
    const navigate = useNavigate();
    const { checkIfLoggedIn, logoutCustomer } = useAuth();
    const [loggedIn, setLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        const checkUserSession = async () => {
            setLoggedIn(await checkIfLoggedIn());
        };

        checkUserSession()
    }, [checkIfLoggedIn]);

    const handleLogout = () => {
        logoutCustomer(); 
        setLoggedIn(false); 
        navigate('/'); 
    };

    const handleSelect = (eventKey: string | null) => {
        if (eventKey === 'account-management') {
            navigate('/profile/management');
        } else if (eventKey === 'user-stats') {
            navigate('/profile/statistics-leaderboard');
        } else if (eventKey === 'user-achievements') {
            navigate('/profile/achievements');
        }
    };

    return (
        <Navbar className="Custom-navbar" bg="dark" variant="dark" expand="lg" fixed="top">
            <Container fluid>
                <Row className="w-100 align-items-center">
                    <Col xs="2">
                        <Navbar.Brand href="/">
                            <img
                                src={logo}
                                width="60"
                                height="50"
                                className="d-inline-block align-top"
                                alt="GuessIt Logo"
                            />
                        </Navbar.Brand>
                    </Col>

                    <Col className="text-center">
                        <Navbar.Brand className="mx-auto text-white">GUESSIT</Navbar.Brand>
                    </Col>

                    <Col xs="2" className="ms-auto">
                        <Nav>
                            {loggedIn ? (
                                <>
                                    <Button variant="outline-light" className="me-2" onClick={() => navigate("/game-dashboard")}>
                                        Play
                                    </Button>
                                    <DropdownButton
                                        id="dropdown-basic-button"
                                        title="Account"
                                        variant="outline-light"
                                        onSelect={handleSelect}
                                        className="me-2"
                                    >
                                        <Dropdown.Item eventKey="account-management">Account Management</Dropdown.Item>
                                        <Dropdown.Item eventKey="user-stats">User Statistics</Dropdown.Item>
                                        <Dropdown.Item eventKey="user-achievements">Achievements</Dropdown.Item>
                                    </DropdownButton>
                                    <Button variant="outline-light" onClick={handleLogout}>
                                        Logout
                                    </Button>
                                </>
                            ) : (
                                <>
                                    <Button variant="outline-light" className="me-2" onClick={() => navigate("/register")}>
                                        Register
                                    </Button>
                                    <Button variant="outline-light" onClick={() => navigate("/login")}>
                                        Log in
                                    </Button>
                                </>
                            )}
                        </Nav>
                    </Col>
                </Row>
            </Container>
        </Navbar>
    );
};

export default CustomNavbar;
