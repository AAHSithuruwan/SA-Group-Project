import React, { useEffect, useState } from 'react';
import './Hero.css'; 
import image from '../../assets/backimage.png'; 
import electronics1 from '../../assets/electronics1.png'
import fashion1 from '../../assets/fashion1.png'
import homegarden1 from '../../assets/home-garden1.png'
import sports1 from '../../assets/sports1.png'
import vehicle1 from '../../assets/vehicle1.png'
import jewellery1 from '../../assets/jewellery1.png'
import antiques1 from '../../assets/antiques1.png'
import arts1 from '../../assets/arts1.png'
import books1 from '../../assets/books1.png'

const Hero = () => {
  const popularItems = [
    {
      id: 1,
      title: 'Gold Jewellery A 22KT',
      price: 'LKR 75000.00',
      imageUrl: image,
      rating: 4,
      timeRemaining: '8d:4h:25m'
    },
    {
      id: 2,
      title: 'Gold Jewellery B 22KT',
      price: 'LKR 80000.00',
      imageUrl: image,
      rating: 5,
      timeRemaining: '7d:6h:40m'
    },
    {
      id: 3,
      title: 'Gold Jewellery C 22KT',
      price: 'LKR 85000.00',
      imageUrl: image,
      rating: 3,
      timeRemaining: '6d:8h:15m'
    },
    {
      id: 4,
      title: 'Gold Jewellery D 22KT',
      price: 'LKR 70000.00',
      imageUrl: image,
      rating: 4,
      timeRemaining: '5d:12h:45m'
    },
    {
      id: 5,
      title: 'Gold Jewellery E 22KT',
      price: 'LKR 90000.00',
      imageUrl: image,
      rating: 5,
      timeRemaining: '4d:2h:35m'
    },
    {
      id: 6,
      title: 'Gold Jewellery F 22KT',
      price: 'LKR 95000.00',
      imageUrl: image,
      rating: 4,
      timeRemaining: '3d:10h:20m'
    }
  ];

  const featuredItems = [
    {
      id: 1,
      title: 'Laptop HP Pavilion',
      price: 'LKR 150000.00',
      imageUrl: electronics1,
      rating: 4,
      timeRemaining: '10d:8h:15m'
    },
    {
      id: 2,
      title: 'Samsung Galaxy S21',
      price: 'LKR 125000.00',
      imageUrl: electronics1,
      rating: 5,
      timeRemaining: '5d:11h:35m'
    },
    {
      id: 3,
      title: 'Nike Air Max Shoes',
      price: 'LKR 35000.00',
      imageUrl: electronics1,
      rating: 4,
      timeRemaining: '3d:6h:20m'
    },
    {
      id: 4,
      title: 'Garden Tools Set',
      price: 'LKR 2500.00',
      imageUrl: electronics1,
      rating: 3,
      timeRemaining: '7d:14h:40m'
    },
    {
      id: 5,
      title: 'Sports Bicycle',
      price: 'LKR 45000.00',
      imageUrl: electronics1,
      rating: 4,
      timeRemaining: '6d:12h:50m'
    },
    {
      id: 6,
      title: 'Toyota Corolla',
      price: 'LKR 3000000.00',
      imageUrl: electronics1,
      rating: 5,
      timeRemaining: '2d:9h:55m'
    }
  ];

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

  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentIndex((prevIndex) => (prevIndex + 1) % carouselItems.length);
    }, 2000); // Slide every 2 seconds
    return () => clearInterval(interval);
  }, [carouselItems.length]);

  const renderStars = (rating) => {
    const stars = [];
    for (let i = 0; i < 5; i++) {
      stars.push(
        <span key={i} className={`star ${i < rating ? 'filled' : ''}`}>&#9733;</span>
      );
    }
    return stars;
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

      {/* Popular Collections Section */}
      <div className="popular-collections">
        <h2>Popular Collections</h2>
        <div className="collection-grid">
          {popularItems.map(item => (
            <div key={item.id} className="collection-item">
              <img src={item.imageUrl} alt={item.title} />
              <h3>{item.title}</h3>
              <div className="rating">{renderStars(item.rating)}</div>
              <p>{item.price}</p>
              <p className="time-remaining">{item.timeRemaining} <span className="more">more</span></p>
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

      {/* Featured Products Section */}
      <div className="featured-products">
        <h2>Featured Products</h2>
        <div className="collection-grid">
          {featuredItems.map(item => (
            <div key={item.id} className="collection-item">
              <img src={item.imageUrl} alt={item.title} />
              <h3>{item.title}</h3>
              <div className="rating">{renderStars(item.rating)}</div>
              <p>{item.price}</p>
              <p className="time-remaining">{item.timeRemaining} <span className="more">more</span></p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Hero;
