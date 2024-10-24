import React, { useEffect, useState } from 'react';
import axios from 'axios'; // Make sure to import axios
import './Hero.css'; 
import image from '../../assets/backimage.png'; 
import electronics1 from '../../assets/electronics1.png';
import fashion1 from '../../assets/fashion1.png';
import homegarden1 from '../../assets/home-garden1.png';
import sports1 from '../../assets/sports1.png';
import vehicle1 from '../../assets/vehicle1.png';
import jewellery1 from '../../assets/jewellery1.png';
import antiques1 from '../../assets/antiques1.png';
import arts1 from '../../assets/arts1.png';
import books1 from '../../assets/books1.png';
import { useLocation, useNavigate } from 'react-router-dom';

const Hero = () => {
  const [auctions, setAuctions] = useState([]);
  const [displayOngoing, setDisplayOngoing] = useState([]);
  const [displayNotStarted, setDisplayNotStarted] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const navigate = useNavigate();

  const carouselItems = [
    { id: 1, title: 'Electronics', imageUrl: electronics1},
    { id: 2, title: 'Fashion', imageUrl: fashion1 },
    { id: 3, title: 'Home & Garden', imageUrl: homegarden1 },
    { id: 4, title: 'Sports', imageUrl: sports1 },
    { id: 5, title: 'Vehicles', imageUrl: vehicle1 },
    { id: 6, title: 'Jewelleries', imageUrl: jewellery1 },
    { id: 7, title: 'Antiques', imageUrl: antiques1 },
    { id: 8, title: 'Arts', imageUrl: arts1 },
    { id: 9, title: 'Books', imageUrl: books1 }
  ];

  const handleAuctionClick = (auction) => {
    navigate('/item-details', { state: { auctionId: auction.auctionId } });
  };

  useEffect(() => {
    const fetchAllAuctions = async () => {
      try {
        const response = await axios.get(`http://localhost:5101/api/Auction/All`);
        const auctions = response.data;

        const currentTime = new Date();

        // Classify auctions
        const ongoing = auctions.filter(auction => 
          new Date(auction.startingDate) <= currentTime && new Date(auction.endDate) > currentTime
        );

        const notStarted = auctions.filter(auction => 
          new Date(auction.startingDate) > currentTime
        );

        const ended = auctions.filter(auction => 
          new Date(auction.endDate) < currentTime
        );

        setAuctions(auctions);
        setDisplayAuctions(ongoing, notStarted, ended);
        window.scrollTo(0, 0);
      } catch (error) {
        console.error('There was an error fetching the auctions!', error);
      }
    };

    const setDisplayAuctions = (ongoing, notStarted, ended) => {
      // Fill ongoing auctions
      let ongoingToDisplay = ongoing.slice(0, 6);
      if (ongoingToDisplay.length < 6) {
        const needed = 6 - ongoingToDisplay.length;
        ongoingToDisplay = ongoingToDisplay.concat(notStarted.slice(0, needed));
      }
      if (ongoingToDisplay.length < 6) {
        const needed = 6 - ongoingToDisplay.length;
        ongoingToDisplay = ongoingToDisplay.concat(ended.slice(0, needed));
      }
      setDisplayOngoing(ongoingToDisplay);

      // Fill not started auctions
      let notStartedToDisplay = notStarted.slice(0, 6);
      if (notStartedToDisplay.length < 6) {
        const needed = 6 - notStartedToDisplay.length;
        notStartedToDisplay = notStartedToDisplay.concat(ongoing.slice(0, needed));
      }
      if (notStartedToDisplay.length < 6) {
        const needed = 6 - notStartedToDisplay.length;
        notStartedToDisplay = notStartedToDisplay.concat(ended.slice(0, needed));
      }
      setDisplayNotStarted(notStartedToDisplay);
    };

    const interval = setInterval(() => {
      setCurrentIndex((prevIndex) => (prevIndex + 1) % carouselItems.length);
    }, 2000); // Slide every 2 seconds

    fetchAllAuctions();
    return () => clearInterval(interval);
  }, [carouselItems.length]);

  useEffect(() => {
    const timer = setInterval(() => {
      setDisplayOngoing(prev => [...prev]); // Update ongoing display to trigger re-render
      setDisplayNotStarted(prev => [...prev]); // Update not started display to trigger re-render
    }, 1000); // Update every second

    return () => clearInterval(timer);
  }, []);

  const renderStars = (rating) => {
    const stars = [];
    for (let i = 0; i < 5; i++) {
      stars.push(
        <span key={i} className={`star ${i < rating ? 'filled' : ''}`}>&#9733;</span>
      );
    }
    return stars;
  };

  const formatTimeRemaining = (endDate) => {
    const now = new Date();
    const timeRemaining = new Date(endDate) - now;

    if (timeRemaining <= 0) return 'Ended';
    
    const days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));
    const hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
    
    return `${days}d ${hours}h ${minutes}m`;
  };

  return (
    <div>
      {/* Hero Section */}
      <div className="hero-container">
        <div className="hero-left">
          <p>
            Welcome to <span className="highlight">BID WAVE</span>, where every bid counts!<br />
            Discover exclusive items, bid in real-time, and secure your next treasure. Start bidding now!
          </p>
        </div>
        <div className="hero-right">
          <img src={image} alt="BidWave" />
        </div>
      </div>

      {/* Popular Collections Section - Ongoing Auctions */}
      <div className="popular-collections">
        <h2>Ongoing Popular Auctions</h2>
        <div className="collection-grid">
          {displayOngoing.map(auction => (
            <div key={auction.id} className="collection-item" onClick={() => handleAuctionClick(auction)}>
              <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} />
              <h3>{auction.productName}</h3><br></br>
              <h4>{`Rs. ${auction.nextBidPrice}`}</h4><br></br>
              <p className="time-remaining">Time Left: {formatTimeRemaining(auction.endDate)}</p>
            </div>
          ))}
        </div>
      </div>

      {/* Carousel Section */}
      <div className="carousel-container">
        <div className="carousel" style={{ transform: `translateX(-${currentIndex * 100}%)` }}>
          {carouselItems.map((item) => (
            <div key={item.id} className="carousel-item">
              <img src={item.imageUrl} alt={item.title} />
              <div className="carousel-text">{item.title}</div>
            </div>
          ))}
        </div>
        <div className="carousel-indicators">
          {carouselItems.map((_, index) => (
            <span
              key={index}
              className={`indicator ${index === currentIndex ? 'active' : ''}`}
            ></span>
          ))}
        </div>
      </div>

      <div className="featured-products">
        <h2>Upcoming Popoular Auctions</h2>
        <div className="collection-grid">
          {displayNotStarted.map(auction => (
            <div key={auction.id} className="collection-item" onClick={() => handleAuctionClick(auction)}>
              <img src={`http://localhost:5101/Images/ProductImages/${auction.productId}.png`} alt={auction.productName} />
              <h3>{auction.productName}</h3><br></br>
              <h4>{`Rs. ${auction.startingPrice}`}</h4><br></br>
              <p className="time-remaining">Starts At: {new Date(auction.startingDate).toLocaleString()}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Hero;





