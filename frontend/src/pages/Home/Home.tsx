import React from 'react';
import { Container, Row, Col, Image } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Home.css';
import earthImage from '../../assets/earth.jpg';

function HomePage() {
  // return (
  //   <Container fluid className="p-0">
  //     <div className="animated-text-container">
  //       <span className="animated-text main-heading">Welcome to GuessIt!</span>
  //       <br />
  //       <span className="animated-text subheading">Discover the world</span>
  //       <br />
  //       <span className="animated-text subheading">and learn geography while having fun!</span>
  //     </div>
  //     <Row className="justify-content-center">
  //       <Col md={20} className="image-wrapper">
  //         <Image src={earthImage} alt="Earth" className="image-container" />
  //       </Col>
  //     </Row>
  //   </Container>
  // );
  return (
    <Container fluid className="p-0">
      <div className="animated-text-container mt-5 pt-5">
        <span className="animated-text main-heading">Welcome to GuessIt!</span>
        <br />
        <span className="animated-text subheading">Discover the world</span>
        <br />
        <span className="animated-text subheading">and learn geography while having fun!</span>
      </div>
      <Row className="justify-content-center">
        <Col md={12} className="image-wrapper">
          <Image src={earthImage} alt="Earth" className="image-container" fluid />
        </Col>
      </Row>
    </Container>
  );
}

export default HomePage;
