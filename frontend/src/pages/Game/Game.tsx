import useLocation from '../../hooks/useLocation';
import { useNavigate, useLocation as useRouterLocation } from 'react-router-dom';
import React, { useState, useEffect, useRef } from 'react';
import { MapContainer, TileLayer, useMapEvents, useMap, Marker, Polyline } from 'react-leaflet';
import { Button, Modal } from 'react-bootstrap';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import './Game.css';
import 'leaflet/dist/leaflet.css';
import * as turf from '@turf/turf';
import Results from '../../components/Results/Results';
import useMonthlyUsage from '../../hooks/useMonthlyUsage';
import { useError } from '../../components/ErrorContext/ErrorContext';
import calculateDistance from '../../assets/DistanceCalculator';

L.Icon.Default.mergeOptions({
	iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
	iconUrl: require('leaflet/dist/images/marker-icon.png'),
	shadowUrl: require('leaflet/dist/images/marker-shadow.png'),
});

interface LatLng {
	lat: number;
	lng: number;
}

interface minMaxOfGeolocation {
	minLat: number;
	maxLat: number;
	minLng: number;
	maxLng: number;
}

const Game = () => {
	var loadedPanoCount = 0;
	var traveledDistance = 0;
	var previousPosition = null;
	var monthlyUsage = 0;
	const panoramaRef = useRef<HTMLDivElement | null>(null);
	const { triggerError } = useError();
	const navigate = useNavigate();
	const [showMap, setShowMap] = useState(false);
	const [showModal, setShowModal] = useState(false);
	const [showResult, setShowResult] = useState(false);
	const [generatedLocation, setGeneratedLocation] = useState<LatLng | null>(null);
	const [selectedLocation, setSelectedLocation] = useState<LatLng | null>(null);
	const [guessedLocation, setGuessedLocation] = useState<LatLng | null>(null);
	const [tempGuessedLocation, setTempGuessedLocation] = useState<LatLng | null>(null);
	const [panoramaFound, setPanoramaFound] = useState(false);
	const [isProcessing, setIsProcessing] = useState(false);
	const [attempts, setAttempts] = useState(0);
	const [distance, setDistance] = useState(0);
	const [score, setScore] = useState(0);
	const [startTime, setStartTime] = useState<Date | null>(null);
	const [endTime, setEndTime] = useState<Date | null>(null);
	const [additionalRange, setAdditionalRange] = useState<number>(450);
	const { fetchGeolocation, geolocation } = useLocation();
	const { getMonthlyUsage, updateMonthlyUsage } = useMonthlyUsage();
	const mapRef = useRef(null);
	const routerLocation = useRouterLocation();
	const MAX_ATTEMPTS = 100;

	useEffect(() => {
		const checkUsage = async () =>{
			monthlyUsage = await getMonthlyUsage();
			if(monthlyUsage > 12500){
				triggerError("Monthly free quota reached")
				navigate('/');
            	return;
			}
		}

		const queryParams = new URLSearchParams(routerLocation.search);
		const geolocationId = queryParams.get('geolocationId');
		const startGame = async () => {
			if (geolocationId) {
				await fetchGeolocation(parseInt(geolocationId));
				console.log(geolocation);
			}
		};

		checkUsage();
		startGame();
	}, []);

	useEffect(() => {
		if (geolocation) {
			findLatAndLngFromGeolocation();
		}
	}, [geolocation]);

	useEffect(() => {
		if (generatedLocation) {
			generatePanorama();
		}
	}, [generatedLocation]);

	useEffect(() => {
		console.log("Attempts: ", attempts);
		if (attempts < MAX_ATTEMPTS && !panoramaFound) {
			findLatAndLngFromGeolocation();
		} else {
			alert('Could not find a suitable location. Please try again.');
		}
	}, [attempts]);

	const getMinMaxOfGeolocation = (): minMaxOfGeolocation => {
		console.log(geolocation)
		var coordinates = null
		if (geolocation!.area.type === 'Polygon') {
			coordinates = geolocation!.area.coordinates[0];
		}
		else {
			coordinates = geolocation!.area.coordinates[0][0];
		}

		let minLat = 90;
		let maxLat = -90;
		let minLng = 180;
		let maxLng = -180;
		console.log(coordinates)

		coordinates.forEach(([lng, lat]: [number, number]) => {
			minLat = Math.min(minLat, lat);
			maxLat = Math.max(maxLat, lat);
			minLng = Math.min(minLng, lng);
			maxLng = Math.max(maxLng, lng);
		});

		return { minLat, maxLat, minLng, maxLng };
	};

	const findLatAndLngFromGeolocation = () => {
		if (geolocation) {
			const { minLat, maxLat, minLng, maxLng } = getMinMaxOfGeolocation();
			let randomPoint = null;
			let isInside = false;

			while (!isInside) {
				const randomLat = Math.random() * (maxLat - minLat) + minLat;
				const randomLng = Math.random() * (maxLng - minLng) + minLng;
				if (randomLat === selectedLocation?.lat && randomLng === selectedLocation?.lng) {
					continue;
				}
				randomPoint = turf.point([randomLng, randomLat]);
				if (geolocation.area.type === 'Polygon') {
					isInside = turf.booleanPointInPolygon(randomPoint, turf.polygon([geolocation.area.coordinates[0]]));
				} else {
					isInside = turf.booleanPointInPolygon(randomPoint, turf.polygon([geolocation.area.coordinates[0][0]]));
				}
			}

			setGeneratedLocation({
				lat: randomPoint!.geometry.coordinates[1],
				lng: randomPoint!.geometry.coordinates[0],
			});
			console.log("Random point generated: ", randomPoint!.geometry.coordinates[1], randomPoint!.geometry.coordinates[0]);
		}
	};

	const generatePanorama = () => {
		setAdditionalRange(prev => prev + 50);
		const sv = new window.google.maps.StreetViewService();
		if (generatedLocation && !isProcessing) {
			setIsProcessing(true);

			const randomPoint = new window.google.maps.LatLng(generatedLocation.lat, generatedLocation.lng);
			console.log("Using LatLng: ", randomPoint.lat(), randomPoint.lng());
			sv.getPanorama({ location: randomPoint, radius: additionalRange }, processSVData);
		}
	};

	const processSVData = (data: google.maps.StreetViewPanoramaData | null, status: google.maps.StreetViewStatus) => {
		console.log(additionalRange);
		console.log(status);

		if (status === window.google.maps.StreetViewStatus.OK && panoramaRef.current && data?.location?.latLng) {
			const pano = new window.google.maps.StreetViewPanorama(panoramaRef.current, {
				position: data.location.latLng,
				pov: { heading: 34, pitch: 10 },
				disableDefaultUI: true,
			});

			pano.addListener('pano_changed', () => {
				loadedPanoCount++;
				console.log("panoramaCount:" + loadedPanoCount);
			})
			pano.addListener('position_changed', () => {
				const currentPosition = pano.getPosition();
				if (previousPosition!) {
					const distance = calculateDistance(
						previousPosition.lat(),
						previousPosition.lng(),
						currentPosition!.lat(),
						currentPosition!.lng()
					);

					traveledDistance += distance;
					console.log(`Traveled distance: ${traveledDistance.toFixed(2)} km`);
				}

				previousPosition = currentPosition;
			});

			previousPosition = pano.getPosition();

			setSelectedLocation({
				lat: data.location.latLng.lat(),
				lng: data.location.latLng.lng(),
			});
			setStartTime(new Date());
			setPanoramaFound(true);
		} else {
			setAttempts(prev => prev + 1);
		}
		setIsProcessing(false);
	};

	const calculateDistanceAndScore = () => {
		if (selectedLocation && guessedLocation) {
			const from = turf.point([selectedLocation.lng, selectedLocation.lat]);
			const to = turf.point([guessedLocation.lng, guessedLocation.lat]);
			const dist = turf.distance(from, to, { units: 'kilometers' });

			setDistance(dist);

			const queryParams = new URLSearchParams(routerLocation.search);
			const gameType = queryParams.get('type');

			let calculatedScore = 0;
			if (gameType === 'country') {
				if (dist <= 50) {
					calculatedScore = 100;
				} else if (dist <= 100) {
					calculatedScore = 50;
				} else if (dist <= 200) {
					calculatedScore = 25;
				} else {
					calculatedScore = 0;
				}
			} else if (gameType === 'continent') {
				if (dist <= 500) {
					calculatedScore = 100;
				} else if (dist <= 1000) {
					calculatedScore = 75;
				} else if (dist <= 2000) {
					calculatedScore = 50;
				} else if (dist <= 5000) {
					calculatedScore = 25;
				} else {
					calculatedScore = 0;
				}
			}
			setScore(calculatedScore);
		}
	};

	useEffect(() => {
		if (guessedLocation) {
			calculateDistanceAndScore();
			setEndTime(new Date());

			setShowResult(true);
		}
	}, [guessedLocation]);

	const handleMapClick = (event: any) => {
		setTempGuessedLocation(event.latlng);
		setShowModal(true);
	};

	const handleConfirmLocation = async () => {
		panoramaRef.current = null;
		await updateMonthlyUsage(loadedPanoCount);
		setGuessedLocation(tempGuessedLocation);
		setShowModal(false);
	};

	const MapClickHandler = () => {
		useMapEvents({
			click: handleMapClick,
		});
		return null;
	};

	const RestrictMapMovement = () => {
		const map = useMap();

		useEffect(() => {
			if (map) {
				const bounds = L.latLngBounds(
					L.latLng(-85, -180),
					L.latLng(85, 180)
				);
				map.setMaxBounds(bounds);
			}
		}, [map]);

		return null;
	};

	const timeElapsed = endTime && startTime ? (endTime.getTime() - startTime.getTime()) / 1000 : 0;

	return (
		<div>
			{!showResult ? (
				<div id="panoramamap" ref={panoramaRef} style={{ width: '100%', height: '100vh' }}>
					<div className={`map-container-animated ${showMap ? 'show' : ''}`}>
						<MapContainer
							center={[20, 0]}
							zoom={2}
							style={{ height: '100%', width: '100%' }}
							ref={mapRef}
						>
							<TileLayer
								// url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
								// attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
								url="https://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png?lang=en"
								attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
							/>
							<MapClickHandler />
							<RestrictMapMovement />
						</MapContainer>

						<Button
							onClick={() => setShowMap(prev => !prev)}
							className="vertical-button"
							variant="primary"
						>
							Toggle World Map
						</Button>
					</div>

					<Modal show={showModal} onHide={() => setShowModal(false)}>
						<Modal.Header closeButton>
							<Modal.Title>Confirm Location</Modal.Title>
						</Modal.Header>
						<Modal.Body>
							Are you sure you want to select this location?
						</Modal.Body>
						<Modal.Footer>
							<Button variant="secondary" onClick={() => setShowModal(false)}>
								Cancel
							</Button>
							<Button variant="primary" onClick={handleConfirmLocation}>
								Yes
							</Button>
						</Modal.Footer>
					</Modal>
				</div>
			) : (
				<Results
					score={score!}
					distance={distance!}
					startTime={startTime!}
					endTime={endTime!}
					timeElapsed={timeElapsed}
					selectedLocation={selectedLocation!}
					traveledDistance={traveledDistance}
					guessedLocation={guessedLocation!}
					gameType={new URLSearchParams(routerLocation.search).get('type')!}
				/>
			)}

		</div>
	);
};

export default Game;




