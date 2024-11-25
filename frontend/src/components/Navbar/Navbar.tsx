import React, { useState, useEffect } from 'react';
import { Navbar, Nav, Container, Row, Col, Button, DropdownButton, Dropdown } from 'react-bootstrap';
import logo from '../../assets/logo_NoBackground.png';
import { useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import "./Navbar.css"

const CustomNavbar = () => {
    const navigate = useNavigate();
    const { checkIfLoggedIn, logoutCustomer } = useAuth();
    const [show, setShow] = useState(false);
    const [loggedIn, setLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        const checkUserSession = async () => {
            setLoggedIn(await checkIfLoggedIn());
        };

        checkUserSession()
    }, [checkIfLoggedIn]);

    const handleLogout = () => {
        setShow(false);
        logoutCustomer();
        setLoggedIn(false);
        navigate('/');
    };

    const handleSelect = (eventKey: string | null) => {
        setShow(false);
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
                <Navbar.Brand onClick={() => navigate("/")} className="d-flex align-items-center">
                    <img
                        src={logo}
                        width="60"
                        height="50"
                        className="d-inline-block align-top me-2"
                        alt="GuessIt Logo"
                    />
                </Navbar.Brand>

                <Navbar.Toggle aria-controls="basic-navbar-nav" onClick={() => setShow(!show)}/>
                <Navbar.Collapse id="basic-navbar-nav" in={show}>
                    <Nav className="ms-auto text-center text-lg-start">
                        {loggedIn ? (
                            <>
                                <Button variant="outline-light" className="me-2 mb-2 mb-lg-0 full-width-button" onClick={() => { setShow(false); navigate("/game-dashboard")}}>
                                    Play
                                </Button>
                                <DropdownButton
                                    id="dropdown-basic-button"
                                    title="Account"
                                    variant="outline-light"
                                    onSelect={handleSelect}
                                    className="me-2 mb-2 mb-lg-0"
                                    menuVariant="dark"
                                >
                                    <Dropdown.Menu className="custom-dropdown-menu">
                                        <Dropdown.Item eventKey="account-management" className="custom-dropdown-item full-width-button">
                                            Account Management
                                        </Dropdown.Item>
                                        <Dropdown.Item eventKey="user-stats" className="custom-dropdown-item full-width-button">
                                            User Statistics
                                        </Dropdown.Item>
                                        <Dropdown.Item eventKey="user-achievements" className="custom-dropdown-item full-width-button">
                                            Achievements
                                        </Dropdown.Item>
                                    </Dropdown.Menu>
                                </DropdownButton>
                                <Button variant="outline-light full-width-button" onClick={handleLogout}>
                                    Logout
                                </Button>
                            </>
                        ) : (
                            <>
                                <Button variant="outline-light" className="me-2 mb-2 mb-lg-0 full-width-button" onClick={() => {setShow(false); navigate("/register")}}>
                                    Register
                                </Button>
                                <Button variant="outline-light" className="me-2 mb-2 mb-lg-0 full-width-button" onClick={() => {setShow(false); navigate("/login")}}>
                                    Log in
                                </Button>
                            </>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default CustomNavbar;
