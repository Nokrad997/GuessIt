import React, { useEffect, useState } from 'react';
import { Card, Container, Row, Col, Spinner, Alert, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import useLocation from '../../hooks/useLocation';

const ITEMS_PER_PAGE = 8;

const Location = () => {
    const { continents, countries, loading, error, fetchContinents, fetchCountries } = useLocation();
    const [currentView, setCurrentView] = useState<'continent' | 'country' | 'city'>('continent');
    const [selectedContinent, setSelectedContinent] = useState<number | null>(null);
    const [selectedCountry, setSelectedCountry] = useState<number | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [isPageLoading, setIsPageLoading] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        fetchContinents();
        console.log(continents);
    }, []);

    const redirectToGame = (id: number | null, type: 'continent' | 'country', geolocationId: number) => {
        if (id !== null) {
            navigate(`game?id=${id}&type=${type}&geolocationId=${geolocationId}`);
        }
    };

    const handleContinentClick = (continentId: number | null, geolocationId: number) => {
        setIsPageLoading(true);
        setTimeout(() => {
            setSelectedContinent(continentId);
            setCurrentView('country');
            setCurrentPage(1);
            if (continentId !== null) fetchCountries(continentId);
            setIsPageLoading(false);
            console.log(countries);
        }, 500);
    };

    const handleCountryClick = (countryId: number | null, geolocationId: number, type: 'continent' | 'country' = 'country') => {
        setIsPageLoading(true);
        setTimeout(() => {
            setSelectedCountry(countryId);
            setCurrentPage(1);
            if (countryId !== null) {
                redirectToGame(countryId, type, geolocationId);
            }
        }, 0);
    };

    const handleNextPage = () => {
        setIsPageLoading(true);
        setTimeout(() => {
            setCurrentPage(prevPage => prevPage + 1);
            setIsPageLoading(false);
        }, 0);
    };

    const handlePreviousPage = () => {
        setIsPageLoading(true);
        setTimeout(() => {
            setCurrentPage(prevPage => Math.max(prevPage - 1, 1));
            setIsPageLoading(false);
        }, 0);
    };

    const paginateItems = (items: Array<{ id: number; name: string; geolocation: number }>) => {
        const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
        const endIndex = startIndex + ITEMS_PER_PAGE;
        return items.slice(startIndex, endIndex);
    };

    const renderCards = (
        items: Array<{ id: number; name: string; geolocation: number }>, 
        onClick: (id: number | null, geolocationId: number) => void,
        wholeLabel: string,
        showWholeOption: boolean,
        continentId: number | null = null,
    ) => {
        const paginatedItems = paginateItems(items);
        return (
            <Row 
                className="justify-content-center mt-4"
                style={{ 
                    display: 'flex', 
                    flexWrap: 'wrap', 
                    gap: '20px', 
                    justifyContent: 'center' 
                }}
            >
                {showWholeOption && (
                    <Col xs={12} sm={6} md={4} lg={2} className="mb-4 d-flex justify-content-center">
                        <Card 
                            onClick={() => {
                                const continent = continents.find(cont => cont.id === continentId);
                                const geolocationId = continent ? continent.geolocation : 0; 
                                handleCountryClick(continentId, geolocationId, 'continent');
                            }} 
                            style={{ 
                                cursor: 'pointer', 
                                minWidth: '200px',  
                                minHeight: '150px', 
                                width: '100%', 
                                maxWidth: '250px',
                                display: 'flex', 
                                alignItems: 'center', 
                                justifyContent: 'center' 
                            }}
                        >
                            <Card.Body className="d-flex align-items-center justify-content-center">
                                <Card.Title className="text-center">{wholeLabel}</Card.Title>
                            </Card.Body>
                        </Card>
                    </Col>
                )}
                {paginatedItems.map(item => (
                    <Col xs={12} sm={6} md={4} lg={3} key={item.id} className="mb-4 d-flex justify-content-center">
                        <Card 
                            onClick={() => onClick(item.id, item.geolocation)}
                            style={{ 
                                cursor: 'pointer', 
                                minWidth: '200px',  
                                minHeight: '150px', 
                                width: '100%', 
                                maxWidth: '250px',
                                display: 'flex', 
                                alignItems: 'center', 
                                justifyContent: 'center' 
                            }}
                        >
                            <Card.Body className="d-flex align-items-center justify-content-center">
                                <Card.Title className="text-center">{item.name}</Card.Title>
                            </Card.Body>
                        </Card>
                    </Col>
                ))}
            </Row>
        );
    };    

    const totalItems = currentView === 'continent' ? continents.length : countries.length;
    const totalPages = Math.ceil(totalItems / ITEMS_PER_PAGE);

    return (
        <Container
            className="py-4 d-flex flex-column align-items-center justify-content-center"
            fluid
            style={{
                minHeight: '100vh',
                background: 'linear-gradient(6deg, rgba(2,0,36,1) 0%, rgba(27,61,134,1) 35%, rgba(0,212,255,1) 100%)',
            }}
        >
            {(loading || isPageLoading) && <Spinner animation="border" />}
            {error && <Alert variant="danger">{error}</Alert>}

            {!isPageLoading && currentView === 'continent' && renderCards(continents, handleContinentClick, '', false)}
            {!isPageLoading && currentView === 'country' && renderCards(countries, handleCountryClick, 'Whole Continent', true, selectedContinent)}

            {totalPages > 1 && !isPageLoading && (
                <div className="mt-4 d-flex justify-content-center">
                    <Button variant="secondary" onClick={handlePreviousPage} disabled={currentPage === 1}>
                        Previous
                    </Button>
                    <span className="mx-3 align-self-center text-white">Page {currentPage} of {totalPages}</span>
                    <Button variant="secondary" onClick={handleNextPage} disabled={currentPage === totalPages}>
                        Next
                    </Button>
                </div>
            )}

            {currentView !== 'continent' && !isPageLoading && (
                <Button
                    variant="secondary"
                    onClick={() => {
                        setCurrentView('continent');
                        setCurrentPage(1);
                    }}
                    className="mt-4"
                >
                    Back to Continents
                </Button>
            )}
        </Container>
    );
};

export default Location;
