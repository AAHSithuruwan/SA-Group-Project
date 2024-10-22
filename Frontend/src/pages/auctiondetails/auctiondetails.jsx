import React from 'react';
import { useNavigate } from 'react-router-dom';
import './auctiondetails.css';
import bag1 from '../../assets/bag.png';
import bag2 from '../../assets/bag.png';
import bag3 from '../../assets/bag.png';

const auctionItems = [
  {
    id: 1,
    image: bag1,
    productName: 'Céline - Classic - Shoulder bag',
    category: 'Fashion',
    startingBidPrice: 'RS:200,000.00',
    highestBidPrice: 'RS:317,000.00',
    highestBidder: 'Nimal Perera',
    biddingDate: '2024/05/06',
    biddingTime: '09.00AM',
    sellerLocation: 'Kalutara, Sri Lanka',
    shippingAddress: '665/L Temple road, Panagoda, Homagama',
    description: `
      Designer: CELINE
      Model: Other
      Condition: Very good condition, slightly used with small signs of wear.
      Scratches on hardware, Minor signs of wear on one base corner, Interior stains
      Size: 24 × 18 cm
      Material: Leather
    `,
  },
  {
    id: 2,
    image: bag2,
    productName: 'Céline - Classic - Shoulder bag',
    category: 'Fashion',
    startingBidPrice: 'RS:200,000.00',
    highestBidPrice: 'RS:317,000.00',
    highestBidder: 'Nimal Perera',
    biddingDate: '2024/05/06',
    biddingTime: '09.00AM',
    sellerLocation: 'Kalutara, Sri Lanka',
    shippingAddress: '665/L Temple road, Panagoda, Homagama',
    description: 
     ` Designer: CELINE
      Model: Other
      Condition: Very good condition, slightly used with small signs of wear.
      Scratches on hardware, Minor signs of wear on one base corner, Interior stains
      Size: 24 × 18 cm
      Material: Leather
    `,
  },

  {
    id: 3,
    image: bag3,
    productName: 'Céline - Classic - Shoulder bag',
    category: 'Fashion',
    startingBidPrice: 'RS:200,000.00',
    highestBidPrice: 'RS:317,000.00',
    highestBidder: 'Nimal Perera',
    biddingDate: '2024/05/06',
    biddingTime: '09.00AM',
    sellerLocation: 'Kalutara, Sri Lanka',
    shippingAddress: '665/L Temple road, Panagoda, Homagama',
    description: 
     ` Designer: CELINE
      Model: Other
      Condition: Very good condition, slightly used with small signs of wear.
      Scratches on hardware, Minor signs of wear on one base corner, Interior stains
      Size: 24 × 18 cm
      Material: Leather
    `,
  },
  // Add more items as needed
];

const AuctionDetails = () => {
  const navigate = useNavigate();

  const handleDetailsClick = (item) => {
    navigate(`/sellerauctionitemdetails/${item.id}`, { state: { item } });
  };

  return (
    <div className="auction-container">
      <div className="auction-details">
        <center><h2>Auction Details</h2></center>
        <table className="auction-table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Status</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {auctionItems.map((item) => (
              <tr key={item.id}>
                <td>
                  <img src={item.image} alt={item.productName} className="auction-image" />
                  <span>{item.productName}</span>
                </td>
                <td>{item.category}</td>
                <td>
                  <button
                    className="details-button"
                    onClick={() => handleDetailsClick(item)}
                  >
                    Details
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default AuctionDetails;
